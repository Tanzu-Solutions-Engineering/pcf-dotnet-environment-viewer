# dotnet-environment-viewer
This application can be modified and deployed from an operating system (Linux, OSX, Windows). This project uses the web project type which does _not_ need to be compiled before deploying, it'll automatically compile on the deployed server on first access.

## CF Instructions
With Windows diego cells deployed and a Windows stack, push the app using manifest defaults:
```
$ cf push
```

## Kubernetes Instructions
These instructions assume you have Windows workers deployed to your k8s cluster. Before you can deploy the application you'll need to build a container image which unfortunately requires Docker on Windows Server 2019.

From your Windows Server with Docker installed, build and push the application to your container registry:

```
PS> docker build -t harbor.run.haas.pez.example.com/library/dotnet-environment-viewer:latest .
PS> docker push harbor.run.haas.pez.example.com/library/dotnet-environment-viewer:latest
```

Create a `envviewer.yml` file with the following contents:
```
apiVersion: v1
kind: Pod
metadata:
  labels:
    run: envviewer
  name: envviewer
spec:
  containers:
  - name: envviewer
    image: harbor.run.haas.pez.example.com/library/dotnet-environment-viewer:latest
    env:
    - name: SQLSERVER_CONNECTION_STRING
      value: Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
  tolerations:
  - key: "windows"
    value: "2019"
    effect: NoExecute
  nodeSelector:
    kubernetes.io/os: windows
```

You'll need update the `SQLSERVER_CONNECTION_STRING` env var or omit the env var if you don't have a DB. Once updated schedule a pod:
```
$ kubectl apply -f envviewer.yml
```