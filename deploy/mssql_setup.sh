#!/bin/bash
set -e

# Suggest running as root or with sudo
if [ "$EUID" -ne 0 ]; then 
  echo "Please run as root or with sudo"
  exit 1
fi

echo "Installing SQL Server 2022 on Rocky Linux..."

# 1. Download the Microsoft Red Hat repository configuration file
sudo curl -o /etc/yum.repos.d/mssql-server.repo https://packages.microsoft.com/config/rhel/9/mssql-server-2022.repo

# 2. Install SQL Server
sudo dnf install -y mssql-server

# 3. Configure SQL Server (This is interactive, sets SA password and Edition)
echo "Running mssql-conf setup..."
echo "You will be prompted to choose an edition (select 2 for Developer) and set the SA password."
sudo /opt/mssql/bin/mssql-conf setup

# 4. Verify service is running
systemctl status mssql-server --no-pager

# 5. Open Firewall ports
echo "Opening port 1433 for SQL Server..."
sudo firewall-cmd --zone=public --add-port=1433/tcp --permanent
sudo firewall-cmd --reload

# 6. Install SQL Server Command-Line Tools (sqlcmd)
echo "Installing sqlcmd tools..."
sudo curl -o /etc/yum.repos.d/msprod.repo https://packages.microsoft.com/config/rhel/9/prod.repo
sudo dnf install -y mssql-tools18 unixODBC-devel

# 7. Add to PATH (for convenience)
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bash_profile
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
source ~/.bashrc

echo "SQL Server installation complete!"
