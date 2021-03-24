FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# The following installs and configured Windows Auth for the app
#RUN powershell.exe Add-WindowsFeature Web-Windows-Auth
#RUN powershell.exe -NoProfile -Command Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/AnonymousAuthentication -name enabled -value false -PSPath 'IIS:\'
#RUN powershell.exe -NoProfile -Command Set-WebConfigurationProperty -filter /system.webServer/security/authentication/windowsAuthentication -name enabled -value true -PSPath 'IIS:\'

# The final instruction copies the site you published earlier into the container.
COPY ./ViewEnvironment/ /inetpub/wwwroot
