# test docker dotnet8

## WSL2 docker

https://gist.github.com/Athou/022c67de48f1cf6584ce6c194af71a09

## run and build

### api

```sh
docker build -f Dockerfile-api -t atlmag/test-net8-web . #225MB
docker build -f Dockerfile-alpine --label test-net8-api --tag test-net8-api:alpine . # 220MB
docker build -f Dockerfile-self-trim --label test-net8-api --tag test-net8-api:self . # 123MB - doesn't always work

docker run -p 8080:8080 test-net8-api
docker run -d --rm -p 8080:8080 --name test-net8-api test-net8-api:self

docker login -u atlmag
docker push atlmag/test-net8-web:latest.

docker build -f Dockerfile-worker -t test-net8-worker --label test-net8-worker --tag test-net8-worker:1.0 .
```


## cli
```sh
docker exec -it test-net8-web1 bash
```

echo "{ \"LogDirectory\": \".\", \"FileSize\": 32768, \"LogLevel\": \"Warning\" }" > OTEL_DIAGNOSTICS.json


## Azure

```sh
az login --use-device-code
az acr login --name

az acr build --registry [registry] --image dotnet8-api .
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

docker run --rm -p 18888:18888 -p 4317:18889 -d --name aspire-dashboard \
mcr.microsoft.com/dotnet/aspire-dashboard:8.0

docker run --rm -it -p 18888:18888 -p 4317:18889 -d --name aspire-dashboard \
    -e DASHBOARD__OTLP__AUTHMODE='ApiKey' \
    -e DASHBOARD__OTLP__PRIMARYAPIKEY='mykey123456789' \
    mcr.microsoft.com/dotnet/aspire-dashboard:8.0.0

## create network
#docker network create net8Network
docker network connect net8Network aspire-dashboard

## run web exporting to otlp
#-e OTEL_EXPORTER_OTLP_METRICS_PROTOCOL='httpProtobuf' \

docker run --rm -p 8080:8080 -d --name test-net8-web1 \
    -e OTEL_EXPORTER_OTLP_HEADERS='x-otlp-api-key=mykey123456789' \
    -e OTEL_EXPORTER_OTLP_ENDPOINT='http://172.18.0.2:4317/' \
test-net8-web:latest

docker network connect net8Network test-net8-web1

# Resilience handling
https://devblogs.microsoft.com/dotnet/building-resilient-cloud-services-with-dotnet-8/