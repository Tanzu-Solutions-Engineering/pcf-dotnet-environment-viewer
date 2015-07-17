cf delete environment -r -f
cf push environment -m 2g -s windows2012R2 -b https://github.com/ryandotsmith/null-buildpack.git --no-start -p ./ViewEnvironment/
cf enable-diego environment
cf start environment
