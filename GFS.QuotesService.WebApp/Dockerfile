FROM ubuntu:22.04

COPY ./publish ./

EXPOSE 5000
ENTRYPOINT ["./GFS.QuotesService.WebApp", "--urls", "http://0.0.0.0:5000"]
