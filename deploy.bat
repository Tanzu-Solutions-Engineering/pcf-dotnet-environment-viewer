export APP_NAME=environment
cf delete $APP_NAME -r -f
cf push $APP_NAME -m 2g -s windows2012R2 -b https://github.com/ryandotsmith/null-buildpack.git --no-start -p ./ViewEnvironment/
cf enable-diego $APP_NAME
cf start $APP_NAME
