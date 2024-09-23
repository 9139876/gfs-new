***
Из интересного:
*** 
Общие библиотеки
- GFS.Api
- GFS.Api.Client
- GFS.BackgroundWorker
- GFS.Common
- GFS.ConsoleLibrary
- GFS.EF
- GFS.WebApplication

Реализации:
- GFS.QuotesService.*
- GFS.ChartService.*

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

Ports:
5000 GFS.QuotesService.WebApp
5100 GFS.QuotesService.BackgroundWorker

6000 ChartService.WebApp


dotnet ef migrations add --startup-project GFS.Portfolio.WebApp/ --project GFS.Portfolio.DAL --context PortfolioDbContext -v Init

dotnet ef migrations add --startup-project GFS.QuotesService.BackgroundWorker/ --project GFS.QuotesService.DAL --context QuotesServiceDbContext -v Init

dotnet ef migrations add --startup-project GFS.ChartService.WebApp/ --project GFS.ChartService.DAL --context ChartServiceDbContext -v Init

add env variable to service TinkoffApiToken - сейчас без swarm, поэтому добавить в переменные окружения на тачке где будет собираться и запускаться $TinkoffApiToken

dotnet publish -c Release -r alpine-arm64 --self-contained true /p:PublishTrimmed=true -o ./publish
docker build . --tag <serviceName:timestamp>
sudo docker save <image> | docker --context orangepi load

# add env variable to service DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
# GFS.QuotesService.BackgroundWorker add env variable to service TinkoffApiToken

front see /data/development/Sandbox/front/my-medium-app
