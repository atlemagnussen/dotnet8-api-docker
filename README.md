# test docker dotnet8

## WSL2 docker

https://gist.github.com/Athou/022c67de48f1cf6584ce6c194af71a09

## run and build

docker build -t test-net8-api .
docker build -f Dockerfile-Api --label digilean-api --tag digilean-api:1.0 .
docker build -f Dockerfile-Server --label digilean-server --tag digilean-server:1.0 .

docker run -p 8080:8080 test-net8-api


## cli

docker exec -it <mycontainer> bash

## Azure

az login --use-device-code
az acr login --name

az acr build --registry digilean --image dotnet8-api .

## optimizing container sizes

Shrinking dotnet container  
https://benfoster.io/blog/optimising-dotnet-docker-images/


Nixos building of containers  
https://nix.dev/tutorials/nixos/building-and-running-docker-images.html

https://klotzandrew.com/blog/smallest-golang-docker-image

## Odata

https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/query-options?tabs=net60

