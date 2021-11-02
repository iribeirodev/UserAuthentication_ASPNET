# User Authentication and Authorization in .NetCore 5 and ASP.NET
Authentication and Authorization with Bearer and JWT 

Database: MongoDB

## Valid API endpoints
### POST: https://localhost:5001/v1/account/login
{
  "username": "test"
  "password": "testpassword"
}

### POST: https://localhost:5001/v1/account/register
{
  "Username": "test"
  "Password": "testpassword"
  "Role": "manager"
}
