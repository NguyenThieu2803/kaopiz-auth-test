# Kaopiz Auth Test

Dự án test cho vị trí .NET Developer tại Kaopiz.  
Ứng dụng bao gồm **Backend (ASP.NET Core API)** và **Frontend (ReactJS)** với các chức năng đăng ký, đăng nhập, phân quyền cơ bản.

###  **Công nghệ sử dụng**
```markdown
## 🛠️ Công nghệ

- Backend: ASP.NET Core 6, Entity Framework Core, JWT Authentication
- Database: SQL Server
- Frontend: ReactJS (Hooks, Axios, React Router)
- Git flow: Feature branches, Develop, Main
## 🚀 Cài đặt & chạy

### Backend
```bash
cd BE_Kaopiz_Test/AuthService.API
dotnet restore
dotnet ef database update   # Tạo database
dotnet run

## 🚀 Cài đặt & chạy

### Backend
```bash
cd BE_Kaopiz_Test/AuthService.API
dotnet restore
dotnet ef database update   # Tạo database
dotnet run

cd FE_Kaopiz_Test
npm install
npm start

