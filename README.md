# JiraOAuthClient

If you need to access data from JIRA you can do it through their REST API which offers several different ways 
to authenticate: Basic, OAuth and via cookies. Basic authentication is handy for quick scripts and and testing, 
but it requires handling plain text credentials. JIRA itself uses cookies to handle authentication best
practices discourage developers from using them. That leaves OAuth which is an industry standard for secure
authentication. This project aims to provide a simple interface for authenticating via OAuth.

Notes:
* This was written targeting Jira Server
* After the request token is acquired the user will need to manually authenticate the request
* The user will be provided a verification token which is needed to acquire the access token
* This project wraps this OAuth library: https://github.com/rhargreaves/oauth-dotnetcore
