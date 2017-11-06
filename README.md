# FeatureToggle
Rest API to manage the toggles and to deliver the toggles values to each client.

---
## EndPoints
[Default](http://localhost:54432/api/toggles)

[Swagger Documentation](http://localhost:54432/swagger/)

---
# Authentication
Authentication is implemented using JWT tokens. The endpoint for creating new tokens can be found [here](http://localhost:54432/api/auth/token)

The default credentials are:

*ADMIN*
{
  "userName": "myAdmin",
  "password": "Administrator!01"
}

*REGULAR USER*
{
  "userName": "pedro",
  "password": "Password!01"
}

---
## Technologies
Implemented using .NET Core 2
