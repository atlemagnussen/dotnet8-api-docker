# test docker dotnet8

## WSL2 docker

https://gist.github.com/Athou/022c67de48f1cf6584ce6c194af71a09

## run and build

### api

```sh
docker build -f Dockerfile-api -t test-net8-api --tag test-net8-api:otlp . #225MB
docker build -f Dockerfile-alpine --label test-net8-api --tag test-net8-api:alpine . # 220MB
docker build -f Dockerfile-self-trim --label test-net8-api --tag test-net8-api:self . # 123MB

docker run -p 8080:8080 test-net8-api
docker run -d --rm -p 8080:8080 --name test-net8-api test-net8-api:self


docker build -f Dockerfile-worker -t test-net8-worker --label test-net8-worker --tag test-net8-worker:1.0 .
```


## cli
```sh
docker exec -it test-net8-api bash
```

## Azure

```sh
az login --use-device-code
az acr login --name

az acr build --registry digilean --image dotnet8-api .
```


## optimizing container sizes

https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained

Shrinking dotnet container  
https://benfoster.io/blog/optimising-dotnet-docker-images/


Nixos building of containers  
https://nix.dev/tutorials/nixos/building-and-running-docker-images.html

https://klotzandrew.com/blog/smallest-golang-docker-image

## Odata

https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/query-options?tabs=net60

## .NET Aspire dashboard

docker pull mcr.microsoft.com/dotnet/aspire-dashboard:8.0

docker run --rm -p 18888:18888 -p 4317:18889 -d --name aspire-dashboard-atle \
mcr.microsoft.com/dotnet/aspire-dashboard:8.0