# Prepare For Automate.....
# bash ./deploy.sh

docker build -t rattawitdev/multi-client-k8s:v1 -f ./Ema.Admin.Web/Dockerfile ./Ema.Admin.Web
docker build -t rattawitdev/multi-server-web-k8s:v1 -f ./Ema.Ijoins.Api/Dockerfile ./Ema.Ijoins.Api
docker build -t rattawitdev/multi-server-mobile-k8s:v1 -f ./Ema.IjoinsChkInOut.Api/Dockerfile ./Ema.IjoinsChkInOut.Api


docker push rattawitdev/multi-client-k8s:v1
docker push rattawitdev/multi-server-web-k8s:v1
docker push rattawitdev/multi-server-mobile-k8s:v1

# Installing and enabling Ingress-Nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.44.0/deploy/static/provider/cloud/deploy.yaml

kubectl apply -f k8s


kubectl set image deployments/client-deployment client=rattawitdev/multi-client-k8s:v1
kubectl set image deployments/server-web-deployment serverweb=rattawitdev/multi-server-web-k8s:v1
kubectl set image deployments/server-mobile-deployment servermobile=rattawitdev/multi-server-mobile-k8s:v1