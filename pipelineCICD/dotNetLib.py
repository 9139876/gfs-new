from xml.dom.minidom import parse


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


def getProjectPackages(projectFile):
    packages = []
    projectDocument = parse(projectFile)
    for packageElement in projectDocument.getElementsByTagName('PackageReference'):
        packages.append(
            {
                "name": packageElement.getAttribute('Include'),
                "version": packageElement.getAttribute('Version')
            })

    return packages
