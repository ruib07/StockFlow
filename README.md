To run this project, you have to complete the following steps:

- Create a .env file on the frontend and put the following:
  - VITE_API_BASE_URL to your backend url;
  - DEV_SERVER_PORT to your frontend port;
  - VITE_API_VERSION to your API version.
    
- Create the database following these steps:
  - Delete Migrations folder in StockFlow.Infrastructure
  - On the terminal, "Add-Migration migration-name"
  - On the terminal, "Update-Database"
  
- You need to be with StockFlow.Server as the StartUp Project and the terminal in the StockFlow.Infrastructure project
- Add a appsettings.json on your StockFlow.Server with the following structure:
```json
{
  "ConnectionStrings": {
    "StockFlowDB": "your connection string"
  },
  "Jwt": {
    "Issuer": "your issuer",
    "Audience": "your audience",
    "Key": "your key"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "your email",
    "Password": "your password",
    "EnableSsl": true
  },
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Forwarded-For",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1s",
        "Limit": 5
      }
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
- Then if you´re using VS2022, you can create a profile that start the StockFlow.Server and the stockflow.client at the same time in the "Configure Startup Projects"

With this steps, you can have the project on your computer without any problem. Have a great day 😄

Home Page No Auth (Dark Mode):
![Dashboard-NoAuth-DarkMode](https://github.com/user-attachments/assets/0c2bcf66-00a8-43e9-a991-7599b2752d9f)

Home Page No Auth (Light Mode):
![Dashboard-NoAuth-LightMode](https://github.com/user-attachments/assets/be2df1b2-085c-4691-a8e5-02fcea5a8ac6)

Home Page (Dark Mode):
![Dashboard-DarkMode](https://github.com/user-attachments/assets/7d4d68a8-f436-4ca1-9e5e-bb74abaf9e7e)

Home Page (Light Mode):
![Dashboard-LightMode](https://github.com/user-attachments/assets/4c525572-d739-4d38-9197-90af87a20546)

Signup Page (Dark Mode):
![Signup-DarkMode](https://github.com/user-attachments/assets/89345346-fe9a-4f27-8fb8-39a85cd745c9)

Signup Page (Light Mode):
![Signup-LightMode](https://github.com/user-attachments/assets/0c378266-82f4-4f6d-8fdc-407a91ea4bc8)

Signin Page (Dark Mode):
![Signin-DarkMode](https://github.com/user-attachments/assets/4b7d01b8-4c33-4478-b849-798667390ea2)

Signin Page (Light Mode):
![Signin-LightMode](https://github.com/user-attachments/assets/b2f02025-f585-4fc2-9893-9548f5acc28b)

Reset Password/Send Email Page (Dark Mode):
![ResetPassword-SendEmail](https://github.com/user-attachments/assets/2c0ed170-610a-4782-bcdb-dac87f180d5e)

Reset Password/Send Email Page (Light Mode):
![ResetPassword-SendEmail](https://github.com/user-attachments/assets/3a4410b3-3fc9-41a0-b351-b31d38d4385e)

Reset Password/Change Password Page (Dark Mode):
![ResetPassword-ChangePassword](https://github.com/user-attachments/assets/0c9f5a8d-0946-4cb4-bab2-b44c7f739bbb)

Reset Password/Change Password Page (Light Mode):
![ResetPassword-ChangePassword](https://github.com/user-attachments/assets/42a48133-672e-475e-aec6-32b928e340d7)

Products Page (Dark Mode):
![ProductsTable-DarkMode](https://github.com/user-attachments/assets/db3fa62a-5186-49e0-8276-14857b02cbbd)

Products Page (Light Mode):
![PurchasesTable-LightMode](https://github.com/user-attachments/assets/bbc4ba3f-8684-4c8b-84f6-85a0f90ba3ab)

Categories Page (Dark Mode):
![CategoriesTable-DarkMode](https://github.com/user-attachments/assets/360c9ae9-f338-4906-84d8-f2771be43502)

Categories Page (Light Mode):
![CategoriesTable-LightMode](https://github.com/user-attachments/assets/2a9056aa-6fb0-489f-bec8-dce4b448459f)

Customers Page (Dark Mode):
![CustomersTable-DarkMode](https://github.com/user-attachments/assets/b159734a-128b-437f-9c11-b9d5513adb9c)

Customers Page (Light Mode):
![CustomersTable-LightMode](https://github.com/user-attachments/assets/d5a64c5a-45b6-410f-9794-cd50b994ea1e)

Suppliers Page (Dark Mode):
![SuppliersTable-DarkMode](https://github.com/user-attachments/assets/b9d68c81-f02e-472d-bb20-b2c040d11e39)

Suppliers Page (Light Mode):
![SuppliersTable-LightMode](https://github.com/user-attachments/assets/2f77318e-3bac-41d6-ad8d-67fc69ef2e9a)

Purchases Page (Dark Mode):
![PurchasesTable-DarkMode](https://github.com/user-attachments/assets/901137f2-80a5-4168-9fb4-2cdaf193f054)

Purchases Page (Light Mode):
![PurchasesTable-LightMode](https://github.com/user-attachments/assets/9f249979-6f89-478c-81d5-276ac7bec62e)

Sales Page (Dark Mode):
![SalesTable-DarkMode](https://github.com/user-attachments/assets/8e757269-7724-44ac-96af-06d5ae6cd431)

Sales Page (Light Mode):
![SalesTable-LightMode](https://github.com/user-attachments/assets/66a4e03f-601e-4445-8f32-100ecda584ac)

Administrator Profile Page (Dark Mode):
![AdminProfile-DarkMode](https://github.com/user-attachments/assets/3f02bbcb-bbda-433a-9140-a2303ee6943b)

Administrator Profile Page (Light Mode):
![AdminProfile-LightMode](https://github.com/user-attachments/assets/d25f0e1e-c8f5-4305-a9d0-35832802583a)













