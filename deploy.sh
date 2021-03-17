docker build -t rattawitdev/multi-client-k8s:latest -f ./Ema.Admin.Web/Dockerfile ./Ema.Admin.Web
docker build -t rattawitdev/multi-server-web-k8s:latest -f ./Ema.Ijoins.Api/Dockerfile ./Ema.Ijoins.Api
docker build -t rattawitdev/multi-server-mobile-k8s:latest -f ./Ema.IjoinsChkInOut.Api/Dockerfile ./Ema.IjoinsChkInOut.Api


docker push rattawitdev/multi-client-k8s:latest
docker push rattawitdev/multi-server-web-k8s:latest
docker push rattawitdev/multi-server-mobile-k8s:latest


kubectl apply -f k8s
kubectl set image deployments/client-deployment client=rattawitdev/multi-client-k8s:latest
kubectl set image deployments/server-web-deployment serverweb=rattawitdev/multi-server-web-k8s:latest
kubectl set image deployments/server-mobile-deployment servermobile=rattawitdev/multi-server-mobile-k8s:latest