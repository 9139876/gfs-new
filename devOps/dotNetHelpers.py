from dataclasses import replace
from importlib.resources import path
from re import template
from unicodedata import name
from xml.dom.minidom import parse
import os
import json

publishSettingsPath = 'publish.last'


class PublishedProject:

    def __init__(self, str):
        splitted = str.split(' ')
        self.hash = splitted[0]
        self.fullPath = splitted[1]
        self.folder = os.path.dirname(self.fullPath)
        self.fileName = os.path.basename(self.fullPath)

    def str(self):
        return self.hash + ' ' + self.fullPath


def unique(array):
    return (list(set(array)))


def getProjectDependencies(projectFile):
    dependencies = [projectFile]
    projectDocument = parse(projectFile)
    for referenceElement in projectDocument.getElementsByTagName('ProjectReference'):
        file = referenceElement.getAttribute(
            'Include').replace('\\', '/')
        dependencies += getProjectDependencies(file)

    return unique(dependencies)


def getProjectDependenciesFolders(projectFile):
    return list(map(lambda item: os.path.basename(os.path.dirname(item)), getProjectDependencies(projectFile)))


def getChangesProjectsFolders(commit):
    changedFiles = os.popen(
        'git diff --name-only {} HEAD'.format(commit)).read().splitlines()

    return unique(list(map(lambda item: os.path.dirname(item), changedFiles)))


def intersect(list1, list2):
    for item1 in list1:
        for item2 in list2:
            if (item1 == item2):
                return True

    return False


def safeSaveFile(filename, payload):
    candidateFileName = filename + '.candidate'
    file = open(candidateFileName, 'w')
    file.write(payload)
    file.close()
    if (os.path.isfile(filename)):
        os.remove(filename)
    os.rename(candidateFileName, filename)


def readJson(path):
    if (os.path.isfile(path)):
        return json.loads(open(path, 'r').read())
    else:
        return json.loads('[]')


def readPublishLast():
    file = open('publish.last', 'r')
    projects = []

    for line in list(map(lambda str: str.strip(), file.read().splitlines())):
        if (len(line) > 0 and line[0] != '#'):
            projects.append(PublishedProject(line))

    file.close()
    return projects


def writePublishLast(projects):
    payload = '\n'.join(list(map(lambda project: project.str(), projects)))
    safeSaveFile(publishSettingsPath, payload)


# def createAndFillDockerFile(project):
#     dependencies = getProjectDependencies(project.fullPath)
#     lines = []

#     for dependency in dependencies:
#         file = os.path.basename(dependency)
#         path = os.path.basename(os.path.dirname(dependency))
#         pathAndFile = os.path.join(path, file)
#         lines.append('COPY ["' + pathAndFile + '", "' + path + '/"]')

#     templateFile = open(os.path.join(
#         project.folder, 'Dockerfile.template'), 'r')
#     template = templateFile.read()
#     templateFile.close()

#     destinationFile = open(os.path.join(project.folder, 'Dockerfile'), 'w')
#     destinationFile.write(template.replace('{{COPY}}', '\n'.join(lines)))
#     destinationFile.close()
