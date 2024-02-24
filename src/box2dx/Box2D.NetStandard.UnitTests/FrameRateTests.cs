 using System;
 using System.Numerics;
 using Box2D.NetStandard.Collision.Shapes;
 using Box2D.NetStandard.Dynamics.Bodies;
 using Box2D.NetStandard.Dynamics.Fixtures;
 using Box2D.NetStandard.Dynamics.World;
 using NUnit.Framework;

 namespace Box2D.NetStandard.UnitTests
 {
     [TestFixture]
     public class FrameRateTests
     {
         
         [TestFixture]
     public class WhenThereIsASingleForce
     {
         [TestCase(60, 30, 240)]
         [TestCase(60, 30, 120)]
         [TestCase(60, 30, 60)]
         [TestCase(60, 30, 1f)]
         [TestCase(60, 15, 240)]
         [TestCase(60, 15, 120)]
         [TestCase(60, 15, 60)]
         [TestCase(60, 15, 1)]
         [TestCase(60, 10, 240)]
         [TestCase(60, 10, 120)]
         [TestCase(60, 10, 60)]
         [TestCase(60, 10, 1)]
         public void DistanceRemainsTheSameWithDifferingFramesPerSecond(int fps1, int fps2, float seconds)
         {
             Vector2 fps1Position = GetPositionAfterSecond(fps1, fps1, seconds);
             Vector2 fps2Position = GetPositionAfterSecond(fps2, fps1, seconds);

             float distance = Vector2.Distance(fps1Position, fps2Position);
             float onePercent = seconds * 0.005f;
             Console.WriteLine(distance + "distance " + onePercent);
             Assert.That(fps1Position.X, Is.EqualTo(fps2Position.X).Within(onePercent),
                 $"Over {seconds} Seconds\nPosition 1 ({fps1}): {fps1Position}\nPosition 2 ({fps2}): {fps2Position}\nDelta: {Math.Abs(Vector2.Distance(fps1Position, fps2Position))}. Approx {Math.Abs(Vector2.Distance(fps1Position, fps2Position)) / seconds} per second off");
             Assert.That(fps1Position.Y, Is.EqualTo(fps2Position.Y).Within(onePercent),
                 $"Over {seconds} Seconds\nPosition 1 ({fps1}): {fps1Position}\nPosition 2 ({fps2}): {fps2Position}\nDelta: {Math.Abs(Vector2.Distance(fps1Position, fps2Position))}. Approx {Math.Abs(Vector2.Distance(fps1Position, fps2Position)) / seconds} per second off");
         }

         private Vector2 GetPositionAfterSecond(int fps, int fps1, float seconds)
         {
             World world = new();
             world.SetGravity(new(0, 0));
             Body boxBody = world.CreateBody(new BodyDef()
             {
                 position = new Vector2(0, 10),
                 type = BodyType.Dynamic,
             });

             boxBody.CreateFixture(new FixtureDef()
             {
                 density = 1.0f,
                 friction = 0.3f,
                 shape = new CircleShape()
                 {
                     // Checked above
                     Radius = 1
                 },
             });
             float delta = 1.0f / fps;
             boxBody.ApplyForceToCenter(new(50, 50));
             for (int i = 0; i < fps * seconds; i++)
             {
                 world.Step((1.0f / fps), 8, 3);
             }

             return world.GetBodyList().GetPosition();
         }
     }

     [TestFixture]
     public class WhenThereIsAConstantForce
     {
         [TestCase(60, 30, 240)]
         [TestCase(60, 30, 120)]
         [TestCase(60, 30, 60)]
         [TestCase(60, 30, 1f)]
         [TestCase(60, 15, 240)]
         [TestCase(60, 15, 120)]
         [TestCase(60, 15, 60)]
         [TestCase(60, 15, 1)]
         [TestCase(60, 10, 240)]
         [TestCase(60, 10, 120)]
         [TestCase(60, 10, 60)]
         [TestCase(60, 10, 1)]
         public void DistanceRemainsTheSameWithDifferingFramesPerSecond(int fps1, int fps2, float seconds)
         {
             Vector2 fps1Position = GetPositionAfterSecond(fps1, fps1, seconds);
             Vector2 fps2Position = GetPositionAfterSecond(fps2, fps1, seconds);

             float distance = Vector2.Distance(fps1Position, fps2Position);
             // This is less reliable due to floating points etc
             float onePercent = seconds * 0.015f;
             Console.WriteLine(distance + "distance " + onePercent);
             Assert.That(fps1Position.X, Is.EqualTo(fps2Position.X).Within(onePercent),
                 $"Over {seconds} Seconds\nPosition 1 ({fps1}): {fps1Position}\nPosition 2 ({fps2}): {fps2Position}\nDelta: {Math.Abs(Vector2.Distance(fps1Position, fps2Position))}. Approx {Math.Abs(Vector2.Distance(fps1Position, fps2Position)) / seconds} per second off");
             Assert.That(fps1Position.Y, Is.EqualTo(fps2Position.Y).Within(onePercent),
                 $"Over {seconds} Seconds\nPosition 1 ({fps1}): {fps1Position}\nPosition 2 ({fps2}): {fps2Position}\nDelta: {Math.Abs(Vector2.Distance(fps1Position, fps2Position))}. Approx {Math.Abs(Vector2.Distance(fps1Position, fps2Position)) / seconds} per second off");
         }

         private Vector2 GetPositionAfterSecond(int fps, int fps1, float seconds)
         {
             World world = new();
             world.SetGravity(new(0, 0));
             Body boxBody = world.CreateBody(new BodyDef()
             {
                 position = new Vector2(0, 10),
                 type = BodyType.Dynamic,
             });

             boxBody.CreateFixture(new FixtureDef()
             {
                 density = 1.0f,
                 friction = 0.3f,
                 shape = new CircleShape()
                 {
                     // Checked above
                     Radius = 1
                 },
             });
             float delta = 1.0f / fps;
          
             for (int i = 0; i < fps * seconds; i++)
             {
                 // must be delta time
                 boxBody.ApplyForceToCenter(new(delta, delta));
                 world.Step((1.0f / fps), 8, 3);
             }

             return world.GetBodyList().GetPosition();
         }
     }
     }
 }