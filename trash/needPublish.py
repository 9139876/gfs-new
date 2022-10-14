import json
import os
from dotNetLib import getProjectDependencies

def unique(array):
    return (list(set(array)))


def intersect(list1, list2):
    for item1 in list1:
        for item2 in list2:
            if (item1 == item2):
                return True

    return False


def getChangesProjects(commit):
    ChangedFiles = os.popen(
        'git diff --name-only {} HEAD'.format(commit)).read().splitlines()

    return unique(ChangedFiles)


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
        '{"project":"' + project + '", "hash":"' + currentHash + '"}'))


if __name__ == "__main__":
    ProjectsToPublish = readProjectsToPublish()
    LastPublish = readLastPublish()
    currentHash = os.popen('git rev-parse HEAD').read().strip()
    NeedPublish = []

    for project in ProjectsToPublish:
        lastHash = getLastPublishHash(LastPublish, project, currentHash)

        if (lastHash is None):
            NeedPublish.append(project)
            lastPublishInsertProjectHash(LastPublish, project, currentHash)
            continue

        dependencies = getProjectDependencies(project)
        changes = getChangesProjects(lastHash)

        if (intersect(dependencies, changes)):
            NeedPublish.append(project)
            lastPublishUpdateProjectHash(LastPublish, project, currentHash)

    for item in NeedPublish:
        print(item)

    f = open("publish.last.candidate", "w")
    f.write(json.dumps(LastPublish))
    f.close()
