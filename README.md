***
GFS - The Grail Forecast System
***

Disclaimer: Плевал я на безопасность (токены, авторизацию и т.д.)!!!

Конфиги зависят от среды, локально Development, при разворачивании на IIS - Production, необходимо указывать в профиле публикации

``
<PropertyGroup>
  <EnvironmentName>Production</EnvironmentName>
</PropertyGroup>
``

Конфиги хранятся в БД, получаются из сервиса Configuration и хранятся локально в виде HotCache.
НАХРЕН НЕ НАДО!!!