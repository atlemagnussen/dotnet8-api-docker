# test docker dotnet8

## WSL2 docker

https://gist.github.com/Athou/022c67de48f1cf6584ce6c194af71a09

## run and build

docker build -t test-net8-api .
docker build -f Dockerfile-Api --label digilean-api --tag digilean-api:1.0 .

docker run -p 8080:8080 test-net8-api

## Azure

az login --use-device-code
az acr login --name

az acr build --registry digilean --image dotnet8-api .

