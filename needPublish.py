import json
from xml.dom.minidom import parse
import os


def unique(array):
    return (list(set(array)))


def intersect(list1, list2):
    for item1 in list1:
        for item2 in list2:
            if (item1 == item2):
                return True

    return False


def getProjectFolder(path):
    return path.split('/')[0].replace('"', '')


def getProjectFolders(list):
    folders = []
    for x in list:
        folders.append(getProjectFolder(x))

    return folders


def getDependencies(projectFile):
    dependencies = [getProjectFolder(projectFile)]
    projectDocument = parse(projectFile)
    for referenceElement in projectDocument.getElementsByTagName('ProjectReference'):
        file = referenceElement.getAttribute(
            'Include').replace('..\\', '').replace('\\', '/')
        dependencies.append(getDependencies(file))

    return dependencies


def getChangesProjects(commit):
    ChangedFiles = os.popen(
        'git diff --name-only {} HEAD'.format(commit)).read().splitlines()
    return unique(getProjectFolders(ChangedFiles))


def readProjectsToPublish():
    file = open('projectsToPublish', 'r')
    lines = []

    for line in file.read().splitlines():
        if (line[0] != '#'):
            lines.append(line)

    file.close()
    return lines


def readLastPublish():
    path = 'publish.last'

    if (os.path.isfile(path)):
        return json.loads(open(path, 'r').read())
    else:
        return json.loads('[]')


def getLastPublishHash(lastPublish, project, currentHash):
    for item in lastPublish:
        if (item['project'] == project):
            return item['hash']

    return None


def lastPublishUpdateProjectHash(lastPublish, project, currentHash):
    for item in lastPublish:
        if (item['project'] == project):
            item['hash'] = currentHash
            return


def lastPublishInsertProjectHash(lastPublish, project, currentHash):
    lastPublish.append(json.loads(
        '{"project":"%a", "hash":"%a"}' % (project, currentHash)))


ProjectsToPublish = readProjectsToPublish()
LastPublish = readLastPublish()
currentHash = os.popen('git rev-parse HEAD').read().strip()
NeedPublish = []

for project in ProjectsToPublish:
    projectFolder = getProjectFolder(project)
    lastHash = getLastPublishHash(LastPublish, projectFolder, currentHash)

    if (lastHash is None):
        NeedPublish.append(projectFolder)
        lastPublishInsertProjectHash(LastPublish, projectFolder, currentHash)
        continue

    dependencies = getDependencies(project)
    changes = getChangesProjects(lastHash)

    if (intersect(dependencies, changes)):
        NeedPublish.append(projectFolder)
        lastPublishUpdateProjectHash(LastPublish, projectFolder, currentHash)

for item in NeedPublish:
    print(item)

f = open("publish.last.candidate", "w")
f.write(json.dumps(LastPublish))
f.close()
