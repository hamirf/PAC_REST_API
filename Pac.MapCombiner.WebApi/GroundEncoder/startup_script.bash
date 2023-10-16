#!/bin/bash

function header_style
{
  echo "                        ____                       "
  echo "                       / __ \____ _   _____  _____ "
  echo "                      / /_/ / __ \ | / / _ \/ ___/ "
  echo "                     / _, _/ /_/ / |/ /  __/ /     "
  echo "                    /_/ |_|\____/|___/\___/_/      "
  echo "                                                   "
  echo "    ______            _____                        __  _            "
  echo "   / ____/___  ____  / __(_)___ ___  ___________ _/ /_(_)___  ____  "
  echo "  / /   / __ \/ __ \/ /_/ / __ \`/ / / / ___/ __ \`/ __/ / __ \/ __ \ "
  echo " / /___/ /_/ / / / / __/ / /_/ / /_/ / /  / /_/ / /_/ / /_/ / / / / "
  echo " \____/\____/_/ /_/_/ /_/\__, /\__,_/_/   \__,_/\__/_/\____/_/ /_/  "
  echo "                        /____/                                      "
  echo ""
}

function set_config
{
  header_style
  # ============================= Get Information ================================
  DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"
  # Rpover Name
  rover_name="$ROVER_NAME"
  default_name=default
  
  # Show Current Information
  echo "Rover Name  :" $rover_name
  echo "Curent Dir  :" $DIR
  echo ""

  # Get Rover Map
  echo "Select Rover Map [1/2/3/..]:"
  search_dir=$DIR/Maps
  count_file=0
  dict_map_list="/"
  for entry in "$search_dir"/*
    do
      count_file=$[$count_file+1]
      maps_name=${entry}/
      map_name=(${maps_name//\// })
      echo "Option : ${count_file}." ${map_name[6]}
      dict_map_list=${dict_map_list}${map_name[6]}/
    done
  
  # split dict map list to array
  map_list=(${dict_map_list//\// })
  len_map_list=${#map_list[@]}

  echo "Input: " 
  read map_option
  echo ""
  
  if [ "$map_option" -le "${len_map_list}" ]; then
    rover_map=${map_list[${map_option}-1]}
  else
    echo "[WARNING] Input Not Recognized, set rover map to default !!!"
    # set to default
    rover_map=forindo_wireframe_table.xml
  fi

  echo "Rover Map   :" $rover_map

  # ============================= Set Configuration ================================
  echo ""
  echo "Setting Up Configuration for rover" $rover_name

  cd $DIR
  file=$DIR/Rovers/$rover_name/${rover_name}_utility_config.json
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_utility_config.json Utility/utility_config.json
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_utility_config.json Utility/utility_config.json
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_encoder_config.json
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_encoder_config.json Odometry/encoder_config.json
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_encoder_config.json Odometry/encoder_config.json
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_intrinsic.xml
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_intrinsic.xml Odometry/intrinsic.xml
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_intrinsic.xml Odometry/intrinsic.xml
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_mask.png
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_mask.png Odometry/mask.png
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_mask.png Odometry/mask.png
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_camera_config.json
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_camera_config.json GFC/camera_config.json
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_camera_config.json GFC/camera_config.json
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_ffc_config.json
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_ffc_config.json FFC/ffc_config.json
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_ffc_config.json FFC/ffc_config.json
  fi

  file=$DIR/Rovers/$rover_name/${rover_name}_spatula.json
  if [ -f "$file" ]; then
    ln -sf $DIR/Rovers/$rover_name/${rover_name}_spatula.json Spatula/spatula.json
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Rovers/${default_name}/${default_name}_spatula.json Spatula/spatula.json
  fi

  file=$DIR/Maps/${rover_map}
  if [ -f "$file" ]; then
    ln -sf $DIR/Maps/${rover_map} Odometry/map.xml
  else 
    echo "This" $file " doest exist, use default configuration instead"
    ln -sf $DIR/Maps/default.xml Odometry/map.xml
  fi

  echo "Configuration Done"
}

set_config
