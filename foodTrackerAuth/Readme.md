# Food Tracker Authentication Lambda - Development Guide

## Project Structure

This Lambda function project consists of:
* **Function.cs** - Main Lambda handler implementing OAuth2 authentication flows with AWS Cognito
* **Models/** - Data models for authentication requests and responses
  * `AuthResponse.cs` - Access and refresh token response model
  * `RefreshTokenRequest.cs` - Refresh token request model
* **aws-lambda-tools-defaults.json** - Default deployment settings for AWS Lambda Tools

## Authentication Endpoints

The Lambda function provides the following endpoints:
- **`/auth`** - Exchange authorization code for tokens
- **`/refresh`** - Refresh access tokens
- **`/userInfo`** - Get user information from Cognito
- **`/logout`** - Revoke tokens

## Environment Variables Required

- `FOOD_TRACKER_AUTH_KEY` - Base64 encoded client secret for Cognito
- `FOOD_TRACKER_CLIENT_ID` - Cognito app client ID

## Development from Visual Studio

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

## Development from Command Line

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

### Install Amazon.Lambda.Tools Global Tools (if not already installed)
```bash
dotnet tool install -g Amazon.Lambda.Tools
```

### Update to latest version (if already installed)
```bash
dotnet tool update -g Amazon.Lambda.Tools
```

### Execute unit tests
```bash
cd "foodTrackerAuth/test/foodTrackerAuth.Tests"
dotnet test
```

### Deploy function to AWS Lambda
```bash
cd "foodTrackerAuth/src/foodTrackerAuth"
dotnet lambda deploy-function
```
