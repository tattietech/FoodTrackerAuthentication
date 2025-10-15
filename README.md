# Food Tracker Authentication Lambda

## Overview

An AWS Lambda function that provides token-based authentication using AWS Cognito for a Food Expiry Tracker application.

## Project Status

**⚠️ Experimental Project - Not in Production**

This project was created as an experiment while studying for AWS certification. The goal was to explore creating a token-based authentication system using AWS Lambda and AWS Cognito. 

**The experiment was successful**, demonstrating that such a system can be effectively implemented with Lambda functions. However, this project is:
- ✅ **Working** - Successfully implements OAuth2 authentication flows with AWS Cognito
- ❌ **Not maintained** - No active development or maintenance
- ❌ **Not live** - Not deployed to production
- ❌ **Not planned for production** - Superseded by other solutions

## Current Food Tracker Status

The current version of my Food Tracker project is using Backend as a Service (BaaS) during development, which provides built-in authentication and authorization features.

## Technical Details

This Lambda function implements the following authentication endpoints:
- **`/auth`** - Exchange authorization code for access and refresh tokens
- **`/refresh`** - Refresh access tokens using refresh tokens
- **`/userInfo`** - Retrieve user information from Cognito
- **`/logout`** - Revoke tokens and logout users

The implementation uses:
- AWS Lambda with API Gateway integration
- AWS Cognito for identity management
- OAuth2 authorization code flow with refresh tokens
- C# (.NET) runtime
