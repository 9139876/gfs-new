from fileinput import filename
from dotNetLib import getProjectPackages
import os
import subprocess

token = '5344bea7059a8250c0a67fb9374046bcade79641'

# search *.csproj
project = '../GFS.QuotesService.Scheduler/GFS.QuotesService.Scheduler.csproj'
packages = getProjectPackages(project)

for package in packages:
    fileName = package['name'] + '.' + package['version'] + '.nupkg'
    filePath = 'tmp/' + fileName
    # url = 'https://www.nuget.org/api/v2/package/' + package['name'] + '/' + package['version']
    url = 'https://www.nuget.org/api/v2/package/{}/{} -L --anyauth'.format(package['name'], package['version'])
    print('loading ' + fileName)
    # os.popen(...).read() - wait complete operation :)
    os.popen('curl {} --output {} >  /dev/null 2>&1'.format(url, filePath)).read()
    
    #check errors!!!!

    os.popen('dotnet nuget push --source gitea --skip-duplicate ' + filePath).read() #--api-key token
    # print(log)
    os.popen('rm '+ filePath).read()
    
print ('loading is done')
# curl http://some.url --output some.file
