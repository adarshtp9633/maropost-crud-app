#!/bin/bash

# Exit on error
set -e

APP_NAME="ModernGridViewCrud"
APP_DIR="/var/www/$APP_NAME"
SERVICE_NAME="kestrel-$APP_NAME.service"

echo "Starting setup for $APP_NAME on Rocky Linux..."

# 1. Install .NET 8 Runtime
echo "Installing .NET 8 Runtime..."
sudo dnf install -y aspnetcore-runtime-8.0

# 2. Install Nginx
echo "Installing Nginx..."
sudo dnf install -y nginx
sudo systemctl enable nginx
sudo systemctl start nginx

# 3. Create Deployment Directory
echo "Creating application directory: $APP_DIR"
sudo mkdir -p $APP_DIR
sudo chown -R $USER:$USER $APP_DIR

# 4. Configure SELinux & Firewall
echo "Configuring SELinux and Firewall..."
# Allow Nginx to relay network traffic
sudo setsebool -P httpd_can_network_connect 1
# Open port 80
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --reload

# 5. Setup Systemd Service
echo "Creating Systemd service..."
sudo bash -c "cat > /etc/systemd/system/$SERVICE_NAME <<EOF
[Unit]
Description=ModernGridViewCrud .NET Web API App

[Service]
WorkingDirectory=$APP_DIR
ExecStart=/usr/bin/dotnet $APP_DIR/ModernGridViewCrud.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=$APP_NAME
User=nginx
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF"

sudo systemctl enable $SERVICE_NAME

# 6. Configure Nginx
echo "Configuring Nginx Reverse Proxy..."
sudo bash -c "cat > /etc/nginx/conf.d/$APP_NAME.conf <<EOF
server {
    listen        80;
    server_name   _;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade \$http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host \$host;
        proxy_cache_bypass \$http_upgrade;
        proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto \$scheme;
    }
}
EOF"

# Validate Nginx config
sudo nginx -t
sudo systemctl reload nginx

echo "Setup complete!"
