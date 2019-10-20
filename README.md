# JiraOAuthClient

To retrieve data from JIRA's REST API a client must authenticate first. JIRA offers several different ways to 
do that: Basic, OAuth and via cookies. Basic authentication is handy for quick scripts and and testing, but it 
requires handling plain text credentials. JIRA itself uses cookies to handle authentication but best practices 
discourage developers from using them. That leaves OAuth which is an industry standard for secure authentication. 
This project aims to provide a simple interface for authenticating with JIRA via OAuth.

See the example console application for a quick start guide on how to use this library.

Notes:
* This project uses the RestSharp Nuget package for web requests
* To make requests after authorization you can either generate a RestSharp Authenticator or build your own manually
with the stored tokens
* This was written for a desktop application targeting Jira Server
* After the request token is acquired the user will need to manually authenticate the request via a web browser
* Despite what you may have read, providing the AccessToken in the Authroization header is not enough to 
access JIRA. You will need either use an OAuth client or manually sign your request

