import os
import dotNetHelpers
import shutil
import time
import calendar

def publish(project):
    baseDirectory = os.getcwd()
    try:
        os.chdir(project.folder)        
        publishPath = './publish'
        
        if os.path.exists(publishPath) and os.path.isdir(publishPath):
            shutil.rmtree(publishPath)

        tag = calendar.timegm(time.gmtime()) # + service name !!! add field to Project
        # dotnet publish -c Release -r alpine-x64 --self-contained true /p:PublishTrimmed=true -o ./publish # x64???
        # sudo docker build . --tag <tag>
        shutil.rmtree(publishPath)
        # copy image to remote
        # remote docker service update --image {tag} {serviceName}
        # remove local images
        # remove remote old images
    finally:
        os.chdir(baseDirectory)


currentHash = os.popen('git rev-parse HEAD').read().strip()

# get need publish projects

lastPublish = dotNetHelpers.readPublishLast()
needPublish = []

for project in lastPublish:
    dependecyFolders = dotNetHelpers.getProjectDependenciesFolders(
        project.fullPath)
    changesFolders = dotNetHelpers.getChangesProjectsFolders(project.hash)
    if (dotNetHelpers.intersect(dependecyFolders, changesFolders)):
        needPublish.append(project)

# publish
index = 0

for project in needPublish:
    try:
        index += 1
        print('Publish ' + str(index) + ' of ' +
              str(len(needPublish)) + ': '+project.fileName)
        publish(project)
        project.hash = currentHash
        dotNetHelpers.writePublishLast(lastPublish)
        print('--Success')
    except:
        print('--Failed :(')
