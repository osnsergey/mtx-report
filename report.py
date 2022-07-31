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
for k in d:
  print("%s. %s" % (k, d[k]))
