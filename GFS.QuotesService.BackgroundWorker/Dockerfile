FROM ubuntu:22.04

COPY ./publish ./

EXPOSE 5100
ENTRYPOINT ["./GFS.QuotesService.BackgroundWorker", "--urls", "http://0.0.0.0:5100"]
