from ast import arg
import sys
from dotNetLib import getProjectDependencies

if __name__ == "__main__":
    if (len(sys.argv) <= 1):
        print("Need enter project!")
        quit()

    dependencies = getProjectDependencies(sys.argv[1])
    dependencies.sort()
    for dep in dependencies:
        print(dep)
