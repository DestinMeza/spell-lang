# Spell Lang
Spell lang is a virtual language that's Developed by Destin Meza to allow users to create their own functionality for spells inside of a video game called. SpellWeaver

## Info

This language is used to make Spells in Spell Weaver. In Spell Weaver spells are the very fabric of the universe. In this world you can manipulate virtually anything. The goal with Spell Lang is to make a true Wizard experience. With a world that is surrounded with knowledge

## Goal

The idea is to make something that works intuitively or eventually will become intuitive. To where anyone can make spells, and not be overwhelmed by the ever increasing complexity.

---

## Core Spell Structure

Knowing the core spell structure will help in adding the creation of new spells. Spells are very complex, and they allow a near endless experience for the user.

### Rules

Below are some of the essential rules that will help when you're creating your spells.

- A spell cannot loop indefinitely, they always will end after 10 cycles.
- A spell's input map cannot be manipulated from another spell.

### Spell Process

A spell processes functions like so. in the picture below.

![FlowChart](/Images/Readme/SpellFlow_01.png "Spell Process - Flow Chart")

### Labels

Labels are what help define the spell. When a person labels their Spell, it helps a person reading it understand what the Spell does. This is mostly useful for spells you intend on sharing with other players. Or even helping yourself out whenever you create several different types of spells.

#### Description Labels

The Format reads in this order from top to bottom like so. 

## Template

```

<SpellName>

<Description>

<Resource>

<Required - Skills>

<Optional - Skills>

<Additional Info>
```

## Example
```
Firebolt

A spell that fires a hot ball of energy that on impact with flammable targets can cause the target to be set ablaze.

Mana

Fire Aspect I

Fire Aspect II

This is a test template.
```
---

## Core Entity Process

Entities are anything that can be spawned inside of the game world. In the game world Entities are comprised of several functional components, that can be specified by the following syntax.

## License

[MIT License](https://choosealicense.com/licenses/mit/)