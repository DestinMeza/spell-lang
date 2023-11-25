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


### How to Create a Spell

Spells are formed in a very readable way. The following is an example of how the language is to be structure to fire a simple firebolt spell.

### Variables
```
//(variable keyword) (variable name) (assignment identifier : "is", "=") (object value to Assign)

//Variables are assigned as such below. 
//Variables are implicitly defined

variable x is Caster's Mana.
variable inputA = Input's LeftMouse.
```

### Entry Point

Entry Points can be any valid entry condition, and this can allow very unique spells to be made.
Entry conditions can be :
- Collision/Triggers
- Input
- On Creation
- On Disposal

```
//(entry keyword : "->")  (entry condition) 

-> inputA's LeftPress

```

### Spell Event Flow

*Look back up at Spell Process to get a better grasp of the different kinds of avalible events to add your spell's functionality*

Understanding Spell Event Flow, is just as important to establishing a well structured spell. Treat spells as a Step By Step Recipe in how they are created. This is also the most important step in spell creation.

#### Casting

In Casting you can add a Resource to the Spell's Resource Pool, by taking from the Player's Resource Pool, or from other things in the game world if you have the skills to allow those aspects. For now below is a basic idea of how you can do that in casting.

Casting has many of possiblities in what you can do. For now we will go over a single line statement.

*Below is the following syntax needed to add a mana to the spell.*

```
//(add keyword) (amount : optional, but default is 1). (Resource) (targetModifer : "from") (target).
//Add 5 mana from the caster to the Spell's mana pool.

OnStartCasting

add 5 Mana from Caster.

```

Now the same, but with a conditional attached at the end. This can seem kind of redundent to do so, but if not added here, the player may when using the spell. Use up more mana than they originally planned to expend. This keeps the Player in control of only using what they have. If you add too little resources than is required to cast spells. Side Effects happen. Sometimes it can be minor. In this case a slight delay will occur in casting speed. Like a cooldown period, but in other areas this could be dangerous.

*Below is the following syntax needed to add a mana to the spell, but only if target has that mana.*


```
//(add keyword) (amount : optional, but default is 1) (Resource) (targetModifer : "from") (target), (conditional).
//Add a mana from the caster to the Spell's mana pool, but only if target has that mana

OnStartCasting

add 5 Mana from Caster, if Caster has it.

```

#### Spell Action

In Spell Action segments you have a few different keywords, but make a bit more sense on why when using them. In Spell Action you have some functions that you can add functionality to just as you can with Casting.

Take mana from the Spell's Resource Pool. Then create a *projectile*. 

*The projectile's speed and mass are dictated, by the keywords used to describe it. Do note, that the keywords added, do not increase anything outright, it only shifts the way the resource is distributed. Which in turn, causes the entity to have different effects.*

```
//(take keyword) (amount : optional, but default is 1) (Resource) (targetModifer : "from") (target), (spawn) (determiner : "a", "an", "the") ("descriptor keywords") (object)
//Take mana from the Spell pool and create a light and fast projectile.

OnStartAction

take 5 Mana from Caster, spawn a "light, fast" Projectile.

```

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