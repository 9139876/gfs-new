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
        self.commented = str[0] == '#'
        if (self.commented):
            str = str[1:]
        splitted = str.split(' ')
        self.serviceName = splitted[0]
        self.hash = splitted[1]
        self.fullPath = splitted[2]
        self.folder = os.path.dirname(self.fullPath)
        self.fileName = os.path.basename(self.fullPath)

    def str(self):
        comment = '#' if self.commented else ''
        return f"{comment}{self.serviceName} {self.hash} {self.fullPath}"


def unique(array):
    return (list(set(array)))


def getProjectDependencies(projectFile):
    dependencies = [projectFile]
    projectDocument = parse(projectFile)
    for referenceElement in projectDocument.getElementsByTagName('ProjectReference'):
        file = referenceElement.getAttribute('Include').replace('\\', '/')
        dependencies += getProjectDependencies(file)

    return unique(dependencies)


def getProjectDependenciesFolders(projectFile):
    return list(map(lambda item: os.path.basename(os.path.dirname(item)), getProjectDependencies(projectFile)))


def getChangesProjectsFolders(commit):
    changedFiles = os.popen('git diff --name-only {} HEAD'.format(commit)).read().splitlines()

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
        if (len(line) > 0):
            projects.append(PublishedProject(line))

    file.close()
    return projects


def writePublishLast(projects):
    payload = '\n'.join(list(map(lambda project: project.str(), projects)))
    safeSaveFile(publishSettingsPath, payload)
