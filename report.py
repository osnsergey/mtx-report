import re
from collections import OrderedDict

def replaceStr(s, fromStr, toStr):
  res=s
  if s.rfind(fromStr) != -1:
    sspl = s.rsplit(fromStr, 1)

    sspl[0] = sspl[0].rstrip()

    if sspl[0][-1] == '.':
      sspl[0] = sspl[0][:-1]
    sspl[0] = sspl[0].rstrip()

    res=toStr.join(sspl)
  return res

def printTasks(title, d):
  if bool(d):
    print()
    print(title)
    for k in d:
      print("%s. %s" % (k, d[k]))

regex = re.compile("^[a-zA-Z]+-[0-9]+ - .+$")
lines=[]

with open("tasks.txt") as f:
  for line in f:
    if regex.search(line) != None:
      lines.append(line.rstrip())

d = {}

for l in lines:
  rsp = re.split('^[a-zA-Z]+-[0-9]+ - ', l)
  lsp = l.split(' - ')

  rsp[1] = replaceStr(rsp[1], ' OPEN', '. (In progress)')
  rsp[1] = replaceStr(rsp[1], ' REOPENED', '. (In progress)')
  rsp[1] = replaceStr(rsp[1], ' IN PROGRESS', '. (In progress)')
  rsp[1] = replaceStr(rsp[1], ' CODE DONE', '. (In progress)')
  rsp[1] = replaceStr(rsp[1], ' READY FOR IMPLEMENTATION', '. (In progress)')
  rsp[1] = replaceStr(rsp[1], ' RESOLVED', '. (Done)')
  rsp[1] = replaceStr(rsp[1], ' CLOSED', '. (Done)')

  d[lsp[0]] = ''.join(rsp)

d = OrderedDict(sorted(d.items()))

ducsx = OrderedDict()
ducs = OrderedDict()
dciwd = OrderedDict()
dnexus = OrderedDict()
dgps = OrderedDict()
dgpsbl = OrderedDict()
dothers = OrderedDict()
  
for k in d:
  if re.match('^UCSX-[0-9]+', k) or re.match('^GAPI-[0-9]+', k):
    ducsx[k]=d[k]
  elif re.match('^ESR-[0-9]+', k) or re.match('^CM-[0-9]+', k):
    ducs[k]=d[k]
  elif re.match('^CIWD-[0-9]+', k):
    dciwd[k]=d[k]
  elif re.match('^NEXUS-[0-9]+', k):
    dnexus[k]=d[k]
  elif re.match('^GPS-[0-9]+', k):
    dgps[k]=d[k]
  elif re.match('^GPSBL-[0-9]+', k):
    dgpsbl[k]=d[k]
  else:
    dothers[k]=d[k]

printTasks('UCS-X:', ducsx)
printTasks('UCS:', ducs)
printTasks('CIWD:', dciwd)
printTasks('NEXUS:', dnexus)
printTasks('G+ SAP Adapter:', dgps)
printTasks('G+ Siebel Adapter:', dgpsbl)
printTasks('Others:', dothers)
