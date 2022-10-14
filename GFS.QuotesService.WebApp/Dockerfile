FROM alpine:3.16.2

RUN apk add --no-cache libstdc++ libintl
    
# Copy 
WORKDIR /app
COPY ./publish ./

EXPOSE 5000
ENTRYPOINT ["./GFS.QuotesService.WebApp", "--urls", "http://0.0.0.0:5000"]

# add env variable to container DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
