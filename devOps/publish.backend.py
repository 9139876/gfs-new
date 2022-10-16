import os
import subprocess
import dotNetHelpers
import shutil
import time
import calendar

# Global variables
publishServerHost = '192.168.1.11'
publishServerLogin = 'orangepi'
remoteMountPath = '/mnt/orangepi_tmp/'


def publish(project, timestamp):
    baseDirectory = os.getcwd()
    remotePath = os.path.join(remoteMountPath, timestamp, project.serviceName)
    tag = project.serviceName + ':' + timestamp

    try:
        os.chdir(project.folder)
        subprocess.call('mkdir ' + remotePath, shell=True)
        subprocess.call('dotnet publish -c Release -r alpine-arm64 --self-contained true /p:PublishTrimmed=true -o ' + os.path.join(remotePath, 'publish'), shell=True)
        subprocess.call('cp ./Dockerfile ' + remotePath, shell=True)

        subprocess.call('docker build ' + remotePath + ' --tag ' + tag, shell=True)
        subprocess.call('docker service update --image ' + tag + ' ' + project.serviceName, shell=True)
    finally:
        os.chdir(baseDirectory)


currentHash = os.popen('git rev-parse HEAD').read().strip()

# get need publish projects

lastPublish = dotNetHelpers.readPublishLast()
needPublish = []

for project in lastPublish:
    dependecyFolders = dotNetHelpers.getProjectDependenciesFolders(project.fullPath)
    changesFolders = dotNetHelpers.getChangesProjectsFolders(project.hash)
    if (dotNetHelpers.intersect(dependecyFolders, changesFolders)):
        needPublish.append(project)

# publish
index = 0

timestamp = str(calendar.timegm(time.gmtime()))
subprocess.call('sshfs ' + publishServerLogin + '@' + publishServerHost + ':/tmp ' + remoteMountPath, shell=True)  # mount
subprocess.call('mkdir ' + os.path.join(remoteMountPath, timestamp), shell=True)

try:
    subprocess.call('docker context use orangepi', shell=True)
    for project in needPublish:
        try:
            index += 1
            print('Publish ' + str(index) + ' of ' + str(len(needPublish)) + ': ' + project.fileName)
            publish(project, timestamp)
            project.hash = currentHash
            dotNetHelpers.writePublishLast(lastPublish)
            print('--Success')
        except:
            print('--Failed :(')
finally:
    subprocess.call('docker context use default', shell=True)
    shutil.rmtree(os.path.join(remoteMountPath, timestamp))
    subprocess.call('fusermount -u ' + remoteMountPath, shell=True)  # unmount
