import ScyTasksInterface as interface
import sys
from time import ctime as clock

todoFile = "F:\\Dropbox\\TODO\\todo.txt"
logFile = "F:\\Dropbox\\TODO\\log.txt"

def sync():
    interface.clearCompletedTasks()
    remoteTasksRaw = interface.getTasks()
    remoteTasks = []
    f = open(todoFile, 'w')
    l = open(logFile, 'a')
    log = ''
    n = 0
    status = ''
    for task in remoteTasksRaw:
        if task['title'] != '' and task['title'] != ' ':
            if n < len(remoteTasksRaw):
                f.write('%s\n' % task['title'])
            else:
                f.write('%s' % task['title'])
            n += 1
            log += '[%s]Wrote %s ...\n' % (clock(), task['title'])
    l.write(log)
    l.close()
    sys.exit()
def main():
    sync()
    ##hold = raw_input('Press enter to exit...')

if __name__ == '__main__':
    main()
        
