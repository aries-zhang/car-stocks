# Car Stocks

## To run

```
dotnet run -p src/car-stocks.csproj
```

Once started, API specs will be available on default Swagger endpoint: http://localhost:5000/swagger 


## To test

```
dotnet test
```

Or, refer to [cars.http](test/scratches/cars.http) for scratching the APIs.

## To build docker image

```
Docker build . -t arieszhang/car-stocks:latest
```


## To deploy

```
kubectl apply -f deploy/deployment-local.yaml
```

Tested on docker desktop Kubernetes only.
