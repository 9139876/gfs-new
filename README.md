***
GFS - The Grail Forecast System
***

Disclaimer: Плевал я на безопасность (токены, авторизацию и т.д.)!!!

Конфиги зависят от среды, локально Development, при разворачивании на сервере - Production, необходимо указывать в профиле публикации

``
<PropertyGroup>
  <EnvironmentName>Production</EnvironmentName>
</PropertyGroup>
``

Конфиги хранятся в БД, получаются из сервиса Configuration и хранятся локально в виде HotCache.
НАХРЕН НЕ НАДО!!!

dotnet ef migrations add --startup-project GFS.Portfolio.WebApp/ --project GFS.Portfolio.DAL --context PortfolioDbContext -v Init

dotnet ef migrations add --startup-project GFS.QuotesService.BackgroundWorker/ --project GFS.QuotesService.DAL --context QuotesServiceDbContext -v Init

add env variable to service TinkoffApiToken - сейчас без swarm, поэтому добавить в переменные окружения на тачке где будет собираться и запускаться $TinkoffApiToken 

dotnet publish -c Release -r alpine-arm64 --self-contained true /p:PublishTrimmed=true -o ./publish
docker build . --tag <serviceName:timestamp>
sudo docker save <image> | docker --context orangepi load
# add env variable to service DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
# GFS.QuotesService.BackgroundWorker add env variable to service TinkoffApiToken

front see /data/development/Sandbox/front/my-medium-app
