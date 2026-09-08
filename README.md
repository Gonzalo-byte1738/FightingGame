Active Ragdoll Kickboxing Controller
A physics-driven, active ragdoll character controller built in Unity. This project combines dual-rig animation matching, dynamic PD torque stabilization, and responsive combat mechanics to produce grounded locomotion, stable stance transitions, and high-impact fighting interactions.

Key Features
Active Ragdoll Locomotion: Hybrid physics setup mapping physical ConfigurableJoint limb chains to an animated target ghost rig.

Fighting Stance Mechanics: Dynamic toggle (T key) switching between free-look camera-relative walking and locked directional combat strafing driven by a 2D Freeform Directional Blend Tree.

PD Torque Balance System: Custom proportional-derivative torque controller (ActiveRagdollKeepUpright) maintaining vertical spine and hip orientation across movement states.

Dynamic Hover Spring: Ground-tracking raycasts sampling foot bone heights to hover the physical hips smoothly and eliminate foot-dragging.  


Adaptive Stance Camera: Multi-profile camera controller (CameraFollow) smoothly transitioning from a third-person perspective to a high-angle isometric combat view upon entering stance.

Foot Lift Assistance: Context-aware vertical impulse forces assisting stepping feet during animation swing phases.

Component Architecture
Core Scripts
PhysicsCharacterController.cs: Manages player input, camera-relative movement vectors, 2D Blend Tree parameter driving (MoveX, MoveY), and stance state toggles.  


ActiveRagdollKeepUpright.cs: Applies PD torque alignment forces to the hips and chest while driving a vertical hover spring anchored to the lowest grounded foot.

SyncGhostToPhysics.cs: Synchronizes the root transform of the animation ghost rig directly to the physical hips to prevent spatial drift.  

CameraFollow.cs: Tracks physical hip position and interpolates offset, FOV, and pitch angles between free locomotion and fighting stance profiles.



Controls
W / A / S / D: Locomotion / Directional Strafing  
CS

T: Toggle Fighting Stance & Isometric View  
CS
