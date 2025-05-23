# Demo-API-Input-Validation
This project contains a tiny ASP.NET Core Web API to demonstrate effective and strict API input validation mechanisms. In detail, it is a practical implementation of a user registration and authentication system. The goal of this sample application is to demonstrate the effectiveness of multi-layered API input validation using a realistic scenario, thereby fostering a deeper understanding of the practical implementation of the best practices discussed above. The API also serves to defend against potential attack vectors such as SQL injection, cross-site scripting (XSS), and the use of weak passwords or invalid input data. The API allows the registration of new users and their subsequent authentication using a username and password. User information is stored persistently in a locally integrated SQLite database. The focus is on validating input data and securely storing sensitive information such as passwords. The API is fully documented and testable via the automatically generated SwaggerUI. The application's architecture is based on the security principle of "defense-in-depth" and is based on several validation layers, described below:

## 1. Syntactic/declarative validation with ASP.NET Data Annotations
Extensive data annotations are used at the model level to check input for formal correctness. The Required attribute ensures that mandatory fields are not submitted empty. The StringLength attribute is used to limit the transmitted character length and prevent denial-of-service attacks caused by excessively long inputs. The RegularExpression attribute was used to implement input whitelisting. This restricts inputs such as the user name to permissible characters (alphanumeric, underscore, hyphen) to prevent injection attacks. Furthermore, the EmailAddress attribute validates whether the specified email format is syntactically correct, so that incomplete or manipulated addresses are rejected. This static validation is performed directly by the ASP.NET model binding and provides initial protection measures when deserializing user input.

## 2. Semantic/logic-based validation with ASP.NET IValidatableObject
The submitted password is additionally validated via the IValidatableObject interface, which enables semantic validation. Password complexity is explicitly checked here. The specified password must contain at least one uppercase letter, one lowercase letter, one number, and one special character, and be at least 14 characters long. These rules comply with current OWASP recommendations and aim to prevent weak passwords that can easily be compromised through dictionary attacks or brute-force methods. Furthermore, it has been technically defined that the application should not allow the use of usernames or email addresses multiple times during user registration. Therefore, before creating a new user account, the database checks whether the submitted username or email address has already been recorded and therefore exists. These measures prevent duplicate registrations and the unintentional overwriting of existing accounts. The verification is performed using LINQ queries, which also minimizes race conditions and timing attacks.

## 3. Persistence-Level Validation
After validation, the submitted user data is stored locally in a SQLite database. The widely used Microsoft Entity Framework Core library (EF Core) is used for all database access and manipulation, which protects against SQL injection attacks by default. In detail, SQL commands are generated internally using secure parameterization. This means that user input is not inserted directly into SQL statements but treated as parameters, effectively preventing the execution of injected SQL commands.

## 4. Secure Password Storage
To securely store passwords, the PBKDF2 key derivation algorithm was used in the Rfc2898DeriveBytes implementation using the SHA-512 hash function. The 100,000 iteration configuration significantly complicates brute-force attacks. Additionally, a unique 32-byte salt is generated for each user using a cryptographically secure random number generator. These security measures ensure that even in the event of a database leak, no direct conclusions can be drawn about the original passwords. The use of salt also effectively protects against rainbow table attacks, since even identical passwords result in different hash values ​​due to different salt values.

<br></br>
## API endpoints (SwaggerUI)
<img src="https://github.com/doupe97/Demo-API-Input-Validation/blob/main/Docs/SwaggerUI-API-endpoints.png">

<br></br>
## User Registration Endpoint
<img src="https://github.com/doupe97/Demo-API-Input-Validation/blob/main/Docs/Registration-Endpoint.png">

<br></br>
## User Login Endpoint
<img src="https://github.com/doupe97/Demo-API-Input-Validation/blob/main/Docs/Login-Endpoint.png">

<br></br>
## SQLite database schema
<img src="https://github.com/doupe97/Demo-API-Input-Validation/blob/main/Docs/Sqlite-Database-Schema.PNG">

<br></br>
## SQLite database content / registered user profiles
<img src="https://github.com/doupe97/Demo-API-Input-Validation/blob/main/Docs/Sqlite-Database-Content.PNG">
