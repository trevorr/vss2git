REM Copyright 2012 Remigius stalder, Descom Consulting Ltd.
REM
REM Licensed under the Apache License, Version 2.0 (the "License");
REM you may not use this file except in compliance with the License.
REM You may obtain a copy of the License at
REM
REM    http://www.apache.org/licenses/LICENSE-2.0
REM
REM Unless required by applicable law or agreed to in writing, software
REM distributed under the License is distributed on an "AS IS" BASIS,
REM WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
REM See the License for the specific language governing permissions and
REM limitations under the License.

md repo
md svn
svnadmin create repo
copy svnserve.conf repo\conf\svnserve.conf /y
copy passwd repo\conf\passwd /y
copy pre-revprop-change.bat repo\hooks\pre-revprop-change.bat /y
