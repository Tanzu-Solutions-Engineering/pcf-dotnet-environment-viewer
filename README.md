# pcf-dotnet-environment-viewer

To push this app, you need to download the diego plugin and deploy to an environment with .NET support.

The command to push.

Push the app with no-start:
'cf push environment -m 2g -s windows2012R2 -b https://github.com/ryandotsmith/null-buildpack.git --no-start -p ./ViewEnvironment/'

Enable diego:
'cf enable-diego environment'

Then Start:
'cf start environment'

Once your app is pushed, you can navigate to the route and you will see all the VCAP variables.  Add ?all= to get all the system variables too.
