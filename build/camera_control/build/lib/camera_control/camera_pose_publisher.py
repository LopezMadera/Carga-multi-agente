#!/usr/bin/env python3

import rclpy
from rclpy.node import Node

from geometry_msgs.msg import Pose

class CameraPublisher(Node):

    def __init__(self):
        super().__init__("camera_pose_publisher")

        self.publisher = self.create_publisher(
            Pose,
            "/camera_pose",
            10
        )

        self.timer = self.create_timer(2.0, self.publish_pose)

        self.i = 0

    def publish_pose(self):

        poses = [
            (0.0,0.0,1.0),
            (1.0,0.0,1.0),
            (1.0,1.0,1.0),
            (0.0,1.0,1.0)
        ]

        pose = Pose()

        pose.position.x = poses[self.i][0]
        pose.position.y = poses[self.i][1]
        pose.position.z = poses[self.i][2]

        pose.orientation.w = 1.0

        self.publisher.publish(pose)

        self.get_logger().info(f"Enviando pose {self.i}")

        self.i = (self.i + 1) % len(poses)

def main():
    rclpy.init()
    node = CameraPublisher()
    rclpy.spin(node)
    node.destroy_node()
    rclpy.shutdown()

if __name__ == "__main__":
    main()