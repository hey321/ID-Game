# ID Card Finding Game - Unity Setup Instructions

## Overview
This is a mini game where players search for an ID card hidden among various items in a messy drawer.

## Scripts Created
1. **DrawerGameController.cs** - Main game controller that manages game state, spawning items, and UI updates
2. **DrawerItem.cs** - Script for individual clickable items in the drawer

## Setup Steps

### 1. Create the UI Canvas
1. Right-click in Hierarchy → **UI → Canvas**
2. This will automatically create a Canvas, EventSystem, and set up the UI system

### 2. Create the Drawer Container
1. Right-click on Canvas → **UI → Image**
2. Rename it to "DrawerContainer"
3. Set the Image component:
   - Color: Brown (#8B4513 or similar)
   - Set RectTransform size: Width 800, Height 500
   - Position it in the center of the screen

### 3. Create the Item Prefab
1. Right-click in Hierarchy → **UI → Image**
2. Rename it to "DrawerItem"
3. Add a **TextMeshPro - Text** as a child:
   - Right-click on DrawerItem → **UI → TextMeshPro - Text**
   - Set alignment to center (both horizontal and vertical)
   - Font size: 48
   - Color: White
4. Add the **DrawerItem** script component to the DrawerItem GameObject
5. Drag DrawerItem from Hierarchy to Project window to create a prefab
6. Delete the DrawerItem from the scene (keep the prefab)

### 4. Create the UI Elements
Create the following UI elements as children of the Canvas (not inside DrawerContainer):

#### Title
1. Right-click on Canvas → **UI → TextMeshPro - Text**
2. Rename to "TitleText"
3. Set text: "🔍 Find the ID Card"
4. Font size: 48
5. Position at top center

#### Game Info Panel
1. Right-click on Canvas → **UI → Image**
2. Rename to "InfoPanel"
3. Set color: Light gray (#F5F5F5)
4. Add a **Horizontal Layout Group** component
5. Set RectTransform: Width 800, Height 80, position below title

#### Info Items (inside InfoPanel)
For each of these, create as children of InfoPanel:
1. **ClicksText** - TextMeshPro - Text: "Clicks: 0"
2. **TimeText** - TextMeshPro - Text: "Time: 0s"
3. **FoundText** - TextMeshPro - Text: "Found: 0/1"

#### Message Text
1. Right-click on Canvas → **UI → TextMeshPro - Text**
2. Rename to "MessageText"
3. Set text: ""
4. Font size: 24
5. Position below DrawerContainer

#### New Game Button
1. Right-click on Canvas → **UI → Button - TextMeshPro**
2. Rename to "NewGameButton"
3. Set button text: "🔄 New Game"
4. Position below MessageText

### 5. Set Up the Game Controller
1. Create an empty GameObject: Right-click in Hierarchy → **Create Empty**
2. Rename it to "GameController"
3. Add the **DrawerGameController** script component
4. In the Inspector, assign the following:
   - **Drawer Container**: Drag the DrawerContainer GameObject
   - **Item Prefab**: Drag the DrawerItem prefab from Project
   - **Clicks Text**: Drag the ClicksText GameObject
   - **Time Text**: Drag the TimeText GameObject
   - **Found Text**: Drag the FoundText GameObject
   - **Message Text**: Drag the MessageText GameObject
   - **New Game Button**: Drag the NewGameButton GameObject

### 6. Configure the DrawerItem Prefab
1. Select the DrawerItem prefab in Project
2. In Inspector, make sure:
   - **Image** component is present
   - **DrawerItem** script is attached
   - The child TextMeshPro text is set up

### 7. Test the Game
1. Press Play
2. Click on items in the drawer to find the ID card
3. The game should track clicks, time, and show messages

## Customization
- Adjust item positions by modifying the random position code in `DrawerGameController.cs`
- Change item list by editing the `items` list in the Inspector
- Modify colors and sizes in the UI elements
- Adjust animations in `DrawerItem.cs`

## Notes
- Make sure TextMeshPro is imported (Unity will prompt you if not)
- The drawer container should have a RectTransform with proper size
- Items are randomly positioned and rotated each game
- The ID card has a special blue color and glow effect


