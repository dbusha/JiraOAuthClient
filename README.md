# JiraOAuthClient

To retrieve data from JIRA's REST API a client must authenticate first. JIRA offers several different ways to do that: Basic, OAuth and via cookies. Basic authentication is handy for quick scripts and and testing, but it requires handling plain text credentials. JIRA itself uses cookies to handle authentication but best practices discourage developers from using them. That leaves OAuth which is an industry standard for secure authentication. This project aims to provide a simple interface for authenticating with JIRA via OAuth.

See the example console application for a quick start guide on how to use this library.

## Dependencies
* This project uses the RestSharp Nuget package 
* Before using this library you must configure JIRA. First generate public and private rsa keys and then, in JIRA, add a new Application Link, specify a ConsumerKey and enter the public key. See the [JIRA documentation](https://developer.atlassian.com/cloud/jira/platform/jira-rest-api-oauth-authentication/) for more information (this link is to the Cloud deployments, but the information about setting up the link is the same in Server instances). 
* This library expects the consumer secret to be the full contents of the private key .pem file. 

## Workflow
1. Query JIRA for the RequestToken
2. Generate a URL that includes the request token parameters
3. Open a web browser, direct your user to the URL and have them accept the request
4. Query JIRA for the AccessToken using the ConsumerKey, ConsumerSecret, RequestToken and RequestTokenSecret
5. Store the AccessToken and AccessToken secret for submitting requests to JIRA without require authentication
6. If you're using RestSharp, generate an Authenticator
7. Submit a request to JIRA using the ConsumerToken, ConsumerSecret, AccessToken, and AccessToken to sign the request

## Notes
* To make requests after authorization you can either generate a RestSharp Authenticator or build your own manually
with the stored tokens
* This was written for a desktop application targeting Jira Server
* After the request token is acquired the user will need to manually authenticate the request via a web browser
* Despite what you may have read, providing the AccessToken in the Authroization header is not enough to 
access JIRA. You will need either use an OAuth client or manually sign your request

