# Mail-Service

The Mail-Service is a lightweight tool designed for effortlessly sending HTML emails. Deployable on your server, it provides a simple and efficient solution for sending HTML emails with support for razor templating.

I built this app to simplify sending HTML emails for job applications and to use it in my web apps for the contact field.

### Key Features

-   **Easy Deployment:** The Mail-Service is designed to be easily deployed on any server. It has minimal dependencies and a straightforward setup process.
-   **Razor Templating Support:** This feature allows you to create dynamic HTML emails with Razor syntax.

## Installation

1. Clone the repository:
   ```sh
   git clone git@github.com:Johnny9Rodriguez/mail-service.git
   ```
2. Navigate to the project directory:
   ```sh
   cd mail-service
   ```
3. Set up your environment variables. Create a `.env` file in the root directory of the project and add your environment variables:

   ```sh
   touch .env
   ```

   Open the `.env` file and add your variables:

   ```
   API_PORT=8010 #default
   TEMPLATE_PATH="./Templates" #default 
   
   Client_SENDER_NAME=your_name
   Client_SENDER_ADDRESS=your_email_address
   
   Client_SMTP_SERVER=your_smtp_server
   Client_SMTP_PORT=your_smtp_port
   
   Client_USERNAME=your_smtp_username
   Client_PASSWORD=your_smtp_password
   ```
4. Restore the .NET packages:
   ```sh
   dotnet restore
   ```
5. Run the application:
   ```sh
   dotnet run
   ```

The application will start and listen for HTTP requests. You can test the API communication by sending a GET request to the root `/` endpoint.

To send an email using the `/example` endpoint, make a POST request with an `ExampleModel` object. The `ExampleModel` object should match the structure defined in [ExampleModel.cs](/Models/ExampleModel.cs).

The email template used for the /example endpoint is located in Example.cshtml. You can modify this template as needed.

