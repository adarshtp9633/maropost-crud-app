# Deployment Guide: ASP.NET Core on GCP Rocky Linux

This guide details the steps to deploy the `ModernGridViewCrud` application to a Google Cloud Platform (GCP) Compute Engine instance running Rocky Linux.

## Prerequisites

1.  **GCP Project**: A Google Cloud project with billing enabled.
2.  **GCP VM Instance**: A Compute Engine instance running **Rocky Linux 8 or 9**.
    *   Ensure HTTP traffic (port 80) is allowed in the firewall settings.
3.  **SQL Server**: A SQL Server instance accessible from the VM. This could be:
    *   Cloud SQL for SQL Server.
    *   A separate VM with SQL Server installed.
    *   (Not Recommended for Prod) A container running on the same VM.
4.  **Local Tools**:
    *   .NET 8 SDK installed.
    *   GCloud SDK (optional, for SCP).
    *   An SFTP client (like FileZilla) or `scp` command.

## Step 1: Configure the Application

The application needs to know how to connect to your production database.

1.  Open `ModernGridViewCrud\appsettings.Production.json`.
2.  Update the `DefaultConnection` string with your real database credentials:
    ```json
    "DefaultConnection": "Server=<YOUR_DB_IP>;Database=EmployeeDb;User Id=<USER>;Password=<PASSWORD>;TrustServerCertificate=True;"
    ```

## Step 2: Publish the Application

We will build the application locally for the Linux enviornment.

1.  Open PowerShell in the project root.
2.  Run the publish script:
    ```powershell
    .\deploy\publish.ps1
    ```
3.  This will create the published files in: `d:\3-tier\Asp.net-CRUD-Operation-Using-ThreeTier-Architecture\bin\Release\net8.0\linux-x64\publish`

## Step 3: Prepare the VM

1.  Connect to your VM via SSH.
2.  Navigate to the directory where you want to upload the setup script (e.g., home directory).
3.  Upload the `deploy/setup_vm.sh` script to the VM.
    ```bash
    # Example using SCP from your local machine
    scp .\deploy\setup_vm.sh username@<VM_EXTERNAL_IP>:~/
    ```
4.  On the VM, make the script executable and run it:
    ```bash
    chmod +x setup_vm.sh
    ./setup_vm.sh
    ```
    *   This script verifies .NET installation, installs Nginx, and configures the systemd service.
    *   **Note**: It expects the app to live in `/var/www/ModernGridViewCrud`.

### Manual Setup Commands (Alternative)

If you prefer to run commands manually instead of using the script:

#### 1. SQL Server 2022 Setup

1.  **Add the Repository**:
    ```bash
    sudo curl -o /etc/yum.repos.d/mssql-server.repo https://packages.microsoft.com/config/rhel/9/mssql-server-2022.repo
    ```

2.  **Install SQL Server**:
    ```bash
    sudo dnf install -y mssql-server
    ```

3.  **Configure Instance (Set Password)**:
    ```bash
    # Follow the prompts to set the SA password and choose edition (e.g., 2 for Developer)
    sudo /opt/mssql/bin/mssql-conf setup
    ```

4.  **Install Command-Line Tools**:
    ```bash
    sudo curl -o /etc/yum.repos.d/msprod.repo https://packages.microsoft.com/config/rhel/9/prod.repo
    sudo dnf install -y mssql-tools18 unixODBC-devel
    echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bash_profile
    source ~/.bash_profile
    ```

5.  **Open Firewall Port 1433**:
    ```bash
    sudo firewall-cmd --zone=public --add-port=1433/tcp --permanent
    sudo firewall-cmd --reload
    ```

#### 2. Application Setup

1.  **Install .NET 8 Runtime**:
    ```bash
    sudo dnf install -y aspnetcore-runtime-8.0
    ```

2.  **Install and Start Nginx**:
    ```bash
    sudo dnf install -y nginx
    sudo systemctl enable nginx
    sudo systemctl start nginx
    ```

3.  **Configure Firewall & SELinux**:
    ```bash
    sudo firewall-cmd --permanent --add-service=http
    sudo firewall-cmd --reload
    sudo setsebool -P httpd_can_network_connect 1
    ```

4.  **Create App Directory**:
    ```bash
    sudo mkdir -p /var/www/ModernGridViewCrud
    sudo chown -R $USER:$USER /var/www/ModernGridViewCrud
    ```

## Step 4: Deploy the Code

1.  Upload the contents of the local `publish` folder to the VM.
    *   You might first upload to your home directory (`~/publish`) and then move it, as you likely won't have direct write access to `/var/www` via SCP/SFTP.

    ```bash
    # Use gcloud compute scp for automatic authentication
    gcloud compute scp --recurse .\bin\Release\net8.0\linux-x64\publish instance-20260128-052642:~/ --zone asia-south1-c
    ```

2.  On the VM, move the files to the final destination:
    ```bash
    sudo cp -r ~/app_files/* /var/www/ModernGridViewCrud/
    ```

3.  Set permissions:
    ```bash
    sudo chown -R nginx:nginx /var/www/ModernGridViewCrud
    sudo chmod -R 755 /var/www/ModernGridViewCrud
    ```

## Step 5: Start the Application

1.  Reload systemd and start the service:
    ```bash
    sudo systemctl daemon-reload
    sudo systemctl restart kestrel-ModernGridViewCrud.service
    ```

2.  Check the status:
    ```bash
    sudo systemctl status kestrel-ModernGridViewCrud.service
    ```

3.  Visit your VM's External IP in a browser. You should see the application.

## Troubleshooting

-   **502 Bad Gateway**: Check if the .NET app is running (`systemctl status kestrel-ModernGridViewCrud.service`).
-   **Database Errors**: Check the logs:
    ```bash
    sudo journalctl -u kestrel-ModernGridViewCrud.service -f
    ```
    Ensure your firewall allows traffic from the VM to the SQL Server (usually port 1433).
