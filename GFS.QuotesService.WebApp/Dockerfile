FROM arm64v8/alpine:3.13

RUN apk add --no-cache libstdc++ libintl

WORKDIR /app
COPY ./publish ./

EXPOSE 5000
ENTRYPOINT ["./GFS.QuotesService.WebApp", "--urls", "http://0.0.0.0:5000"]
