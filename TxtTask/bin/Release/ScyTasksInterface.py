#!/usr/bin/python

import gflags
import httplib2

from apiclient.discovery import build
from oauth2client.file import Storage
from oauth2client.client import OAuth2WebServerFlow
from oauth2client.tools import run

FLAGS = gflags.FLAGS

FLOW = OAuth2WebServerFlow(
    client_id='363050479362.apps.googleusercontent.com',
    client_secret='XGbmhLcQiiWvI_T7FeK5dLjX',
    scope='https://www.googleapis.com/auth/tasks',
    user_agent='ScyTasks/1.0')

storage = Storage('tasks.dat')
credentials = storage.get()
if credentials is None or credentials.invalid == True:
    credentials = run(FLOW, storage)

http = httplib2.Http()
http = credentials.authorize(http)

service = build(serviceName='tasks', version='v1', http=http,
                developerKey='AIzaSyCVn_nyWY271x2_3wqOoF7ph_fJ25mHt4o')

def getTaskListID(taskListName):
    tasklists = service.tasklists().list().execute()
    for tasklist in tasklists['items']:
        if tasklist['title'] == taskListName:
            return tasklist['id']

def getTaskLists():
    tasklists = service.tasklists().list().execute()
    taskLists = []
    for tasklist in tasklists['items']:
        taskLists.append(tasklist['title'])
    return taskLists

def getTaskList(taskListID):
    tasklist = service.tasklists().get(tasklist=taskListID).execute()
    return tasklist['title']

def createTaskList(title):
    newTaskList = { 'title': title }
    result = service.tasklists().insert(body=newTaskList).execute()
    return result['id']

def updateTaskList(taskListID, newTitle):
    tasklist = service.tasklists().get(tasklist=taskListID).execute()
    tasklist['title'] = newTitle
    result = service.tasklists().update(tasklist=tasklist['id'], body=tasklist).execute()
    return result['title']

def deleteTaskList(taskListID):
    service.tasklists().delete(tasklist=taskListID).execute()

def getTasks():
    tasks = service.tasks().list(tasklist='@default').execute()
    tasksList = []
    for task in tasks['items']:
        tasksList.append(task)
    return tasksList

def getTask(taskID):
    task = service.tasks().get(tasklist='@default', task=taskID).execute()
    return task['title']

def getTaskID(string):
    tasklist = service.tasks().list(tasklist='@default').execute()
    for task in tasklist['items']:
        if unicode.encode(task['title']) == string: return task['id']

def createTask(title, notes, due):
    task = { 'title': title, 'notes': notes, 'due': due}
    result = service.tasks().insert(tasklist='@default', body=task).execute()
    return result['id']

def updateTask(taskID):
    task = service.tasks().get(tasklist='@default', task=taskID).execute()
    task['status'] = 'completed'

    result = service.tasks().update(tasklist='@default', task=task['id'], body=task).execute()
    return result['completed']

def orderTask(taskID, parentTaskID, previousTaskID):
    result = service.tasks().move(tasklist='@default', task=taskID, parent=parentTaskID, previous=previousTaskID).execute()
    return result['parent'], result['position']

def deleteTask(taskID):
    service.tasks().delete(tasklist='@default', task=taskID).execute()

def clearCompletedTasks():
    body = service.tasklists().get(tasklist='@default').execute()
    service.tasks().clear(tasklist='@default', body=body).execute()
def customAction(stringToEval):
    if stringToEval.count('\n') > 0:
        toEval = stringToEval.split('\n')
        for i in toEval:
            eval(i)
    else:
        eval(stringToEval)
