#!/bin/bash

FILE="Properties/AssemblyInfo.cs"
VERSION=$(cat $FILE | grep AssemblyVersion  | awk -F\" '{print $2}' | awk -F\" '{print $1}')

VER_MAJ=$(echo $VERSION | awk -F. '{print $1}')
VER_MIN=$(echo $VERSION | awk -F. '{print $2}')
VER_BLD=$(echo $VERSION | awk -F. '{print $3}')
VER_REV=$(echo $VERSION | awk -F. '{print $4}')

DATE_Y=$(date +"%y")
DATE_D=$(date +"%j" | sed "s/^0*\(.*\)/\1/")


if [ "$VER_MIN" != "$DATE_Y" ] || [ "$VER_BLD" != "$DATE_D" ]; then
    VER_MIN=$DATE_Y
    VER_BLD=$DATE_D
    VER_REV=0
else
    VER_REV=$(($VER_REV+1))
fi

VERSION="$VER_MAJ.$VER_MIN.$VER_BLD.$VER_REV"

sed -i 's/AssemblyVersion(".*")/AssemblyVersion("'$VERSION'")/' $FILE
